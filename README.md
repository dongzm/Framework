# .NET 数据库连接操作类
## 用法
>1、在主项目中引用libs的DDTek.Oracle.dll和Framework.dll，DDTek.Oracle.dll是用来代替用来代替原来的oracel的连接类。

>2、把DBConnection\DDTek.lic放到在主项目的DBConnection目录下，这个是licence，必须要有。

>3、原来代码生成器生成的代码中需要修改所有DAL中的代码，在继承类后面增加DDTek,如下，我是保留了原来的功能，如果你觉得每次修改麻烦，可以直接修改源码，或者在写一个代码生成器。

>4、数据库连接字符串需要修改，新的格式：Host=192.168.1.6;Port=1521;Service Name=orc;User ID=uername;Password=password;
```C#
public class BusinessregisterDao : BaseDaoOraDDTek<Businessregister, BusinessregisterCollection>
{

}
```

>5、类似有直接操作到数据库的地方（如事务、存储过程）都需要在原来的类名后面增加DDTek
```C#
var resultList =
                    ProcedureOperationDDTek.RunProcedure<P_ComExlImpCondition, P_ComExlImpResult>(
                        condition);
```

## 优点
>不需要在像原来那样在客户端需要安装oracle服务\客户端才能连接数据库

## 缺点
>这个是项目是收费的，我们用的是破解版
