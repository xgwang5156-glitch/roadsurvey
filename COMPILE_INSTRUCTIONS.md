# 🔨 RoadSurvey Windows 版本编译指南

## 📋 系统要求

### 最低要求
- Windows 10 或更新版本
- 2GB RAM
- 500MB磁盘空间

### 开发要求
- .NET 8.0 SDK
- Visual Studio 2022 或 Visual Studio Code
- C# 12 支持

## 🚀 编译步骤

### 方式1：使用命令行（推荐）

#### 第1步：安装.NET SDK

1. 访问 https://dotnet.microsoft.com/download
2. 下载 ".NET 8.0 SDK"
3. 运行安装程序

验证安装：
```bash
dotnet --version
dotnet --list-runtimes
```

#### 第2步：进入项目目录

```bash
cd RoadSurveyWPF
```

#### 第3步：还原依赖包

```bash
dotnet restore
```

这会下载所有必需的NuGet包：
- System.Data.SQLite (1.0.118.0)
- DocumentFormat.OpenXml (3.0.0)
- OpenXmlPowerTools (4.5.14)

#### 第4步：编译项目

**Debug版本（用于测试）：**
```bash
dotnet build
```

**Release版本（性能优化）：**
```bash
dotnet build -c Release
```

#### 第5步：运行应用

```bash
dotnet run
```

或直接运行编译后的exe：
```bash
.\bin\Release\net8.0-windows\RoadSurvey.exe
```

---

### 方式2：使用Visual Studio 2022

#### 第1步：打开项目

1. 打开Visual Studio 2022
2. File → Open → 选择RoadSurveyWPF文件夹
3. 等待项目加载

#### 第2步：编译

Build → Build Solution（或按Ctrl+Shift+B）

#### 第3步：运行

Debug → Start Debugging（或按F5）

---

## 📦 生成发布版本

### 生成单个.exe文件

这是最常见的发布方式。

```bash
dotnet publish -c Release -r win-x64 --self-contained -p:PublishSingleFile=true
```

输出文件：
```
bin/Release/net8.0-windows/win-x64/publish/RoadSurvey.exe
```

### 生成便携版（推荐）

包含所有依赖，可在任何Windows 10+电脑上运行，无需安装.NET。

```bash
dotnet publish -c Release -r win-x64 --self-contained -p:SelfContained=true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true
```

文件大小约：100-150MB（包含.NET Runtime）

### 生成依赖版（最小）

需要目标电脑已安装.NET 8.0 Runtime。

```bash
dotnet publish -c Release -r win-x64 --no-self-contained -p:PublishSingleFile=true
```

文件大小约：20-30MB

## 🔧 常见问题

### 问题1：找不到dotnet命令

**原因**：.NET SDK未安装或PATH未更新

**解决**：
```bash
# 重新启动命令行
# 或重新安装.NET SDK
# 或检查PATH环境变量
echo %PATH%
```

### 问题2：编译错误 - "Could not load type..."

**原因**：依赖包版本冲突

**解决**：
```bash
dotnet clean
dotnet restore --no-cache
dotnet build
```

### 问题3：编译错误 - "The project file does not exist"

**原因**：不在正确的目录

**解决**：
```bash
cd RoadSurveyWPF  # 确保在项目目录
dotnet build
```

### 问题4：NuGet包下载失败

**原因**：网络问题或NuGet源

**解决**：
```bash
# 清除NuGet缓存
dotnet nuget locals all --clear

# 使用国内源（如阿里云）
dotnet nuget add source https://mirrors.aliyun.com/nuget/v3/index.json -n aliyun

# 重新还原
dotnet restore
```

### 问题5：运行时缺少依赖

**原因**：不是自包含版本，未安装运行时

**解决**：
```bash
# 安装.NET 8.0 Runtime
# 访问：https://dotnet.microsoft.com/download/dotnet/8.0

# 或使用自包含版本
dotnet publish -c Release -r win-x64 --self-contained
```

## 🐛 调试技巧

### 启用详细日志

```bash
# 编译时显示详细信息
dotnet build -v d

# 发布时显示详细信息
dotnet publish -v d
```

### 调试运行

```bash
# 使用调试器运行
dotnet run --configuration Debug
```

### 查看编译输出

```bash
# 打开输出目录
.\bin\Debug\net8.0-windows\
```

## 🔐 代码签名（可选）

对于正式发布，可以对.exe进行数字签名：

```bash
# 需要证书文件
signtool sign /f certificate.pfx /p password /t http://timestamp.server.com RoadSurvey.exe
```

## 📊 编译输出

### Debug版本
- 位置：`bin/Debug/net8.0-windows/`
- 大小：约10-20MB
- 速度：快速启动

### Release版本
- 位置：`bin/Release/net8.0-windows/`
- 大小：约5-10MB（部署版）或100-150MB（自包含版）
- 性能：最优化

## ✅ 编译检查清单

- [ ] .NET 8.0 SDK已安装
- [ ] 进入了RoadSurveyWPF目录
- [ ] 运行了 `dotnet restore`
- [ ] 运行了 `dotnet build` 成功
- [ ] 运行了 `dotnet run` 应用正常启动
- [ ] 功能测试正常
- [ ] 可生成Word报告

## 📝 发布清单

对于最终发布：

- [ ] 使用Release配置编译
- [ ] 生成自包含版本的exe
- [ ] 测试所有功能
- [ ] 创建ReadMe文档
- [ ] 准备安装说明
- [ ] 备份源代码
- [ ] 更新版本号

## 🚀 部署到目标电脑

### 方式1：直接复制exe

最简单的方式（如果使用自包含版本）：

1. 将`RoadSurvey.exe`复制到目标位置
2. 双击运行即可

### 方式2：创建安装程序

```bash
# 使用NSIS或WiX Toolset创建msi安装程序
# 需要额外配置
```

### 方式3：使用portable版本

1. 生成Release版本
2. 复制整个publish文件夹
3. 用户可从U盘或网络运行

## 📞 技术支持

如遇编译问题：

1. 检查.NET版本：`dotnet --version`
2. 查看详细错误：`dotnet build -v d`
3. 清除缓存：`dotnet clean && dotnet restore --no-cache`
4. 查看官方文档：https://learn.microsoft.com/dotnet/

---

**编译成功后，您将获得可在任何Windows 10+电脑上运行的应用！** 🎉
