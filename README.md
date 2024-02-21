# GS 电子班牌控制后台 - 3.0

无需多盐。

## 程序结构

- 插件程序集 `Cirno.ChinaGS.Injection.Permanent.dll`
- 破解用程序集（随 release 附带） `../GS.Terminal.SmartBoard.Logic_v3.dll`
- 扩展功能包接口程序集 `CirnoFramework.Interface.Commands.dll`
- 扩展功能包文件夹 `CommandLib/`
- 基础扩展功能包 `CommandLib/CirnoBuiltins.dll`
- 扩展功能包管理器 `CommandLib/CirnoPM.dll`
- 相关配置文件

## 使用说明

### 1.安装

1. 首先，使用 `../GS.Terminal.SmartBoard.Logic_v3.dll` 替换掉班牌安装目录下 `Addons/GS.Terminal.SmartBoard.Logic.dll`，需要备份原始文件，需要将破解程序集重命名为 `GS.Terminal.SmartBoard.Logic.dll`
2. 将 `./Cirno.ChinaGS.Injection.Permanent` 目录复制到班牌安装目录下 `Addons/` 文件夹
3. 重启班牌程序

### 2. 指令发送

使用 dnSpy 查看扩展功能包中的形如

```csharp
[Export(typeof(ICommand))]
[Command("Builtin.ShadowLantern")]
```

的内容，将 `[Command("Builtin.ShadowLantern")]` 括号中内容，如 `Builtin.ShadowLantern`，添加到 `command_parser.py` 中字典对象 `command` 中，如：

```python
commands = {
    0: "Builtin.ShadowLantern",
}
```

确保 `command_parser.py` 同一路径下有一个包含了以 CRLF 换行分隔的，名为 `IpList.txt` 的 IP 列表（此版本未上传，目前为旧版本，默认发送到 `127.0.0.1`），执行 `python command_parser.py`。

### 3. 扩展包开发

略

更新内容：

- 可动态更新的指令列表
- 屎山代码重构与修复
- 优化逻辑

Jan 20 2024
