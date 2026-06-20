using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using RoadSurvey.Data;
using RoadSurvey.Models;
using RoadSurvey.Services;
using System.Text.Json;

namespace RoadSurvey;

public partial class MainWindow : Window
{
    private DatabaseManager? _dbManager;
    private string _currentProjectPath = "";
    private Project? _currentProject;

    public MainWindow()
    {
        InitializeComponent();
        InitializeEventHandlers();
        LoadRecentProjects();
    }

    private void InitializeEventHandlers()
    {
        NewProjectBtn.Click += NewProject_Click;
        OpenProjectBtn.Click += OpenProject_Click;
        SaveProjectBtn.Click += SaveProject_Click;
        ExportWordBtn.Click += ExportWord_Click;
        SaveProjectInfoBtn.Click += SaveProjectInfo_Click;
        AddBridgeBtn.Click += AddBridge_Click;
        AddObstacleBtn.Click += AddObstacle_Click;
        AddOtherObstacleBtn.Click += AddOtherObstacle_Click;
        UploadRouteMapBtn.Click += UploadRouteMap_Click;
    }

    private void LoadRecentProjects()
    {
        // 此处可以实现加载最近打开的项目列表
    }

    private void NewProject_Click(object sender, RoutedEventArgs e)
    {
        var saveDialog = new SaveFileDialog
        {
            FileName = "NewProject.db",
            Filter = "数据库文件 (*.db)|*.db",
            Title = "创建新项目"
        };

        if (saveDialog.ShowDialog() == true)
        {
            _currentProjectPath = saveDialog.FileName;
            InitializeDatabase();
            _currentProject = new Project();
            RefreshUI();
            MessageBox.Show("项目创建成功！", "提示");
        }
    }

    private void OpenProject_Click(object sender, RoutedEventArgs e)
    {
        var openDialog = new OpenFileDialog
        {
            Filter = "数据库文件 (*.db)|*.db",
            Title = "打开项目"
        };

        if (openDialog.ShowDialog() == true)
        {
            _currentProjectPath = openDialog.FileName;
            InitializeDatabase();
            LoadProject();
            RefreshUI();
        }
    }

    private void InitializeDatabase()
    {
        _dbManager = new DatabaseManager(_currentProjectPath);
        _dbManager.InitializeDatabase();
    }

    private void LoadProject()
    {
        if (_dbManager == null) return;
        _currentProject = _dbManager.GetProject();
        
        if (_currentProject != null)
        {
            ProjectNameLabel.Text = $"项目: {_currentProject.ProjectName}";
            ProjectNameTextBox.Text = _currentProject.ProjectName;
            CargoParamsTextBox.Text = _currentProject.CargoParams;
            VehicleConfigTextBox.Text = _currentProject.VehicleConfig;
            SurveyorTextBox.Text = _currentProject.Surveyor;
            SurveyDateTextBox.Text = _currentProject.SurveyDate;
            StartPointTextBox.Text = _currentProject.StartPoint;
            EndPointTextBox.Text = _currentProject.EndPoint;
            DistanceTextBox.Text = _currentProject.Distance;
            RemarksTextBox.Text = _currentProject.Remarks;
            LogisticsSummaryTextBox.Text = _currentProject.LogisticsSummary;
        }
        else
        {
            ProjectNameLabel.Text = "新项目";
        }

        RefreshDataGrids();
    }

    private void SaveProject_Click(object sender, RoutedEventArgs e)
    {
        SaveProjectInfo_Click(sender, e);
        MessageBox.Show("项目已保存！", "提示");
    }

    private void SaveProjectInfo_Click(object sender, RoutedEventArgs e)
    {
        if (_dbManager == null)
        {
            MessageBox.Show("请先创建或打开一个项目", "提示");
            return;
        }

        if (string.IsNullOrWhiteSpace(ProjectNameTextBox.Text))
        {
            MessageBox.Show("项目名称不能为空！", "错误");
            return;
        }

        if (_currentProject == null)
        {
            _currentProject = new Project();
        }

        _currentProject.ProjectName = ProjectNameTextBox.Text;
        _currentProject.CargoParams = CargoParamsTextBox.Text;
        _currentProject.VehicleConfig = VehicleConfigTextBox.Text;
        _currentProject.Surveyor = SurveyorTextBox.Text;
        _currentProject.SurveyDate = SurveyDateTextBox.Text;
        _currentProject.StartPoint = StartPointTextBox.Text;
        _currentProject.EndPoint = EndPointTextBox.Text;
        _currentProject.Distance = DistanceTextBox.Text;
        _currentProject.Remarks = RemarksTextBox.Text;
        _currentProject.LogisticsSummary = LogisticsSummaryTextBox.Text;
        _currentProject.UpdatedAt = DateTime.Now;

        if (_currentProject.Id == 0)
        {
            _dbManager.InsertProject(_currentProject);
        }
        else
        {
            _dbManager.UpdateProject(_currentProject);
        }

        ProjectNameLabel.Text = $"项目: {_currentProject.ProjectName}";
    }

    private void ExportWord_Click(object sender, RoutedEventArgs e)
    {
        if (_dbManager == null || _currentProject == null)
        {
            MessageBox.Show("请先创建或打开一个项目", "提示");
            return;
        }

        var saveDialog = new SaveFileDialog
        {
            FileName = $"{_currentProject.ProjectName}_报告.docx",
            Filter = "Word文档 (*.docx)|*.docx",
            Title = "导出Word报告"
        };

        if (saveDialog.ShowDialog() == true)
        {
            try
            {
                var bridges = _dbManager.GetBridges();
                var obstacles = _dbManager.GetObstacles();
                var otherObstacles = _dbManager.GetOtherObstacles();

                WordGenerator.GenerateReport(saveDialog.FileName, _currentProject, bridges, obstacles, otherObstacles);
                MessageBox.Show($"报告已导出到: {saveDialog.FileName}", "成功");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"导出失败: {ex.Message}", "错误");
            }
        }
    }

    private void UploadRouteMap_Click(object sender, RoutedEventArgs e)
    {
        var openDialog = new OpenFileDialog
        {
            Filter = "图像文件 (*.jpg;*.png;*.bmp)|*.jpg;*.png;*.bmp",
            Title = "选择路线图"
        };

        if (openDialog.ShowDialog() == true)
        {
            if (_currentProject == null)
                _currentProject = new Project();

            _currentProject.RouteMapPath = openDialog.FileName;
            MessageBox.Show("路线图已上传！", "提示");
        }
    }

    private void AddBridge_Click(object sender, RoutedEventArgs e)
    {
        if (_dbManager == null)
        {
            MessageBox.Show("请先创建或打开一个项目", "提示");
            return;
        }

        if (string.IsNullOrWhiteSpace(BridgeKmTextBox.Text))
        {
            MessageBox.Show("K值不能为空！", "错误");
            return;
        }

        try
        {
            var kmNumeric = ExtractKmNumeric(BridgeKmTextBox.Text);
            var bridge = new Bridge
            {
                Name = BridgeNameTextBox.Text,
                KmValue = BridgeKmTextBox.Text,
                KmNumeric = kmNumeric,
                RoadNumber = (BridgeRoadComboBox.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "",
                Type = (BridgeTypeComboBox.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "",
                Span = BridgeSpanTextBox?.Text ?? "",
                Width = BridgeWidthTextBox?.Text ?? ""
            };

            _dbManager.InsertBridge(bridge);
            RefreshDataGrids();
            ClearBridgeInputs();
            MessageBox.Show("桥梁已添加！", "成功");
        }
        catch (Exception ex)
        {
            MessageBox.Show($"添加失败: {ex.Message}", "错误");
        }
    }

    private void AddObstacle_Click(object sender, RoutedEventArgs e)
    {
        if (_dbManager == null)
        {
            MessageBox.Show("请先创建或打开一个项目", "提示");
            return;
        }

        if (string.IsNullOrWhiteSpace(ObstacleKmTextBox.Text))
        {
            MessageBox.Show("K值不能为空！", "错误");
            return;
        }

        try
        {
            var kmNumeric = ExtractKmNumeric(ObstacleKmTextBox.Text);
            var obstacle = new Obstacle
            {
                Type = (ObstacleTypeComboBox.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "",
                Name = ObstacleNameTextBox.Text,
                KmValue = ObstacleKmTextBox.Text,
                KmNumeric = kmNumeric,
                MinHeight = ObstacleHeightTextBox.Text
            };

            _dbManager.InsertObstacle(obstacle);
            RefreshDataGrids();
            ClearObstacleInputs();
            MessageBox.Show("障碍已添加！", "成功");
        }
        catch (Exception ex)
        {
            MessageBox.Show($"添加失败: {ex.Message}", "错误");
        }
    }

    private void AddOtherObstacle_Click(object sender, RoutedEventArgs e)
    {
        if (_dbManager == null)
        {
            MessageBox.Show("请先创建或打开一个项目", "提示");
            return;
        }

        if (string.IsNullOrWhiteSpace(OtherKmTextBox.Text))
        {
            MessageBox.Show("K值不能为空！", "错误");
            return;
        }

        try
        {
            var kmNumeric = ExtractKmNumeric(OtherKmTextBox.Text);
            var obstacle = new OtherObstacle
            {
                Name = OtherNameTextBox.Text,
                KmValue = OtherKmTextBox.Text,
                KmNumeric = kmNumeric,
                RoadNumber = (OtherRoadComboBox.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "",
                RoadHeightDifference = OtherHeightTextBox.Text
            };

            _dbManager.InsertOtherObstacle(obstacle);
            RefreshDataGrids();
            ClearOtherObstacleInputs();
            MessageBox.Show("其他障碍已添加！", "成功");
        }
        catch (Exception ex)
        {
            MessageBox.Show($"添加失败: {ex.Message}", "错误");
        }
    }

    private void ModuleListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (ModuleListBox.SelectedItem is ListBoxItem item)
        {
            ContentTabControl.SelectedIndex = ModuleListBox.SelectedIndex;
        }
    }

    private void RefreshDataGrids()
    {
        if (_dbManager == null) return;

        var bridges = _dbManager.GetBridges();
        var obstacles = _dbManager.GetObstacles();
        var otherObstacles = _dbManager.GetOtherObstacles();

        BridgesDataGrid.ItemsSource = bridges;
        ObstaclesDataGrid.ItemsSource = obstacles;
        OtherObstaclesDataGrid.ItemsSource = otherObstacles;

        BridgeCountLabel.Text = $"桥梁数: {bridges.Count}";
        ObstacleCountLabel.Text = $"限高/涵洞数: {obstacles.Count}";
        OtherObstacleCountLabel.Text = $"其他障碍数: {otherObstacles.Count}";
    }

    private void RefreshUI()
    {
        RefreshDataGrids();
    }

    private void ClearBridgeInputs()
    {
        BridgeNameTextBox.Clear();
        BridgeKmTextBox.Clear();
    }

    private void ClearObstacleInputs()
    {
        ObstacleNameTextBox.Clear();
        ObstacleKmTextBox.Clear();
        ObstacleHeightTextBox.Clear();
    }

    private void ClearOtherObstacleInputs()
    {
        OtherNameTextBox.Clear();
        OtherKmTextBox.Clear();
        OtherHeightTextBox.Text = "0.2-0.4m";
    }

    private double ExtractKmNumeric(string kmValue)
    {
        var cleanValue = kmValue.Replace("K", "").Replace("k", "").Trim();
        return double.TryParse(cleanValue, out var result) ? result : 0;
    }
}
