#Excel配置工具

- 不处理正在打开的Excel文件
- 只识别EC结尾的Excel文件
- 只处理第一行第一列为GenerateExcelCsharpCode的表单 第一行第二列为集合类型 List或Dictionary
- 第二行备注 第三行字段类型 第四行字段名 第五行开始配置数据
- 在第一次导入或手动Reimport文件触发自动转换
- 自动转换会生成对应的Csharp文件和集中放置的Json文件 [Assets/ECJsonData]