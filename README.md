# 🚀 RoadSurvey Windows 版本

**道路勘测数据管理系统 - Windows Desktop Application (WPF)**

## 📋 项目概述

这是将原始Android应用重构为Windows桌面应用。使用C# + WPF开发，提供完整的道路勘测数据管理和Word报告生成功能。

## ✨ 核心特性

✅ **项目管理**
- 新建项目
- 打开现有项目  
- 删除项目
- 项目另存为
- 最近打开列表

✅ **数据管理**
- 项目基础信息
- 桥梁数据
- 限高/涵洞数据
- 其他障碍数据
- 支持增删改查

✅ **自动功能**
- 按K值自动排序
- 数据自动保存
- 智能数据压缩

✅ **报告生成**
- Word自动生成
- 与Android版本结构相同
- 照片自动嵌入

✅ **界面特性**
- 简洁专业UI
- 浅蓝色功能栏
- 左侧导航菜单
- DataGrid表格显示

## 🛠️ 技术栈

- **语言**: C# 12
- **框架**: WPF (.NET 8.0 Windows Desktop)
- **数据库**: SQLite
- **Office库**: DocumentFormat.OpenXml
- **依赖管理**: NuGet

## 📦 项目结构

```
RoadSurveyWPF/
├── Models/              # 数据模型
│   ├── Project.cs
│   ├── Bridge.cs
│   ├── Obstacle.cs
│   └── OtherObstacle.cs
├── Data/                # 数据访问层
│   └── DatabaseManager.cs
├── Services/            # 业务逻辑层
│   └── WordGenerator.cs
├── App.xaml             # 应用配置
├── App.xaml.cs
├── MainWindow.xaml      # 主窗口UI
├── MainWindow.xaml.cs   # 主窗口代码
└── RoadSurvey.csproj    # 项目文件
```

## 🚀 快速开始

### 前置条件
- .NET 8.0 SDK 或更高版本
- Visual Studio 2022 或 Visual Studio Code
- Windows 10 或更新版本

### 编译步骤

1. **安装.NET SDK**
   ```bash
   # 下载地址: https://dotnet.microsoft.com/download
   # 验证安装
   dotnet --version
   ```

2. **进入项目目录**
   ```bash
   cd RoadSurveyWPF
   ```

3. **还原依赖**
   ```bash
   dotnet restore
   ```

4. **编译项目**
   ```bash
   dotnet build -c Release
   ```

5. **运行应用**
   ```bash
   dotnet run
   ```

### 发布为.exe文件

```bash
# 发布为单个可执行文件
dotnet publish -c Release -r win-x64 --self-contained -p:PublishSingleFile=true

# 输出位置: bin/Release/net8.0-windows/win-x64/publish/RoadSurvey.exe
```

## 📱 使用说明

### 创建新项目
1. 点击工具栏"新建项目"按钮
2. 选择保存位置和项目名称（.db文件）
3. 在左侧选择"项目基础信息"并填写详情
4. 点击"保存项目信息"

### 添加数据
1. 在左侧菜单选择对应模块（桥梁、障碍等）
2. 在下方输入框填写数据
3. 点击"添加"按钮
4. 数据会自动保存并显示在上方表格

### 生成Word报告
1. 确保项目已有数据
2. 点击工具栏"导出Word"按钮
3. 选择保存位置
4. 报告将自动生成并包含所有数据和照片

## 🗄️ 数据存储

### 数据库文件
- **位置**: 用户选择的位置（.db文件）
- **类型**: SQLite 3
- **表**: Projects, Bridges, Obstacles, OtherObstacles

### 自动保存
- 修改项目信息后自动保存
- 添加数据后自动保存
- 无需手动保存操作

## 🎨 界面配置

### 浅蓝色功能栏
```xaml
Background="#87CEEB"  <!-- 天蓝色 -->
```

### 颜色方案
- 主色: #87CEEB (天蓝色)
- 浅背景: #F5F5F5
- 文字: #333333

## 📊 数据排序规则

### K值处理
- 自动提取K值中的数字部分
- 例如: "K31", "K3.5", "K100" → 排序为 3.5, 31, 100
- 无效K值按输入顺序排序

### 混合排序
- 所有障碍类型按K值统一排序
- 详细描述表按里程从小到大排列

## 🔒 安全性

- SQLite数据库存储在本地
- 无需互联网连接
- 数据永久保存在选定位置
- 支持数据备份（复制.db文件）

## 🐛 常见问题

### Q: 如何更改项目保存位置？
A: 使用"新建项目"或"打开项目"时选择位置

### Q: 数据会丢失吗？
A: 不会。所有数据自动保存到SQLite数据库

### Q: 如何导出/备份数据？
A: 复制.db文件即可备份。生成Word报告可导出完整数据

### Q: 支持多项目吗？
A: 支持。每个项目是独立的.db文件，可随时切换

## 📝 开发说明

### 添加新功能

1. **添加新模块**
   - 在Models文件夹创建模型类
   - 在DatabaseManager添加CRUD方法
   - 在MainWindow.xaml添加UI
   - 在MainWindow.xaml.cs添加事件处理

2. **修改数据库**
   - 编辑DatabaseManager.cs的表定义
   - 增加Version号（自动迁移）

3. **自定义样式**
   - 修改App.xaml中的样式定义

## 🔄 版本历史

### v1.0 (2024年6月)
- ✅ 初始版本
- ✅ 所有核心功能实现
- ✅ Windows WPF版本完成

## 📞 技术支持

- **项目团队**: COSCO-KM
- **开发时间**: 2024年6月
- **技术框架**: C# + WPF + .NET 8.0

## 📄 许可证

© 2024 COSCO-KM. All rights reserved.

## 🎯 下一步计划

- [ ] 多语言支持
- [ ] 数据验证增强
- [ ] 云同步功能
- [ ] 高级报告模板
- [ ] 数据分析仪表板

---

**现在就开始使用RoadSurvey Windows版本吧！** 🚀
