# .NET 数据库连接操作类
## 用法
>1、在主项目中应用libs的DDTek.Oracle.dll和Framework.dll，DDTek.Oracle.dll是用来代替用来代替原来的oracel的连接类。

>2、把DBConnection\DDTek.lic放到在主项目的DBConnection目录下，这个是licence，必须要有。

>3、原来代码生成器生成的代码中需要修改所有DAL中的代码，在继承类后面增加DDTek,如下，我是保留了原来的功能，如果你觉得每次修改麻烦可以直接修改源码，或者在写一个代码生成器。

```C#
public class BusinessregisterDao : BaseDaoOraDDTek<Businessregister, BusinessregisterCollection>
{

}
```
## 优点
>不需要在像原来那样在客户端需要安装oracle服务\客户端才能连接数据库

## 缺点
>这个是项目是收费的，我们用的是破解版
