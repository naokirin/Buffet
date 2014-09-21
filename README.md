# Buffet

Excelに定義されたテーブルデータをSQLiteに変換することを目的としたライブラリです。

このライブラリはCrocellとRoughlySQLiteの2つのプロジェクトで構成されています。

## Crocell

CrocellはExcelからデータを取得することを目的としたライブラリです。

まずExcelのテーブルに対応するクラスを定義します。
Excelのシートは次のようになっていることを仮定しています。

シート名:foo_bar

|              | A            | B            | C            |
|:------------:|:------------:|:------------:|:------------:|
|       1      |defined_column|       ID     |     Name     |
|       2      |              |       0      |      foo     |
|       3      |              |       1      |      bar     |


次のようにこのシートに対応するクラスを定義します。

```
// defined_columnのある行がカラム名の定義行
[Sheet("foo_bar", DefinedColumn="defined_column")]
class FooBar
{
      public int ID { get; set; }
          public string Name { get; set; }
}
```

そして、このExcelを読み込むことで、各行ごとのデータを取得できるようになります。

```
using(var wb = new XLWorkbook("foo_bar.xlsx"))
{
      List<FooBar> data = wb.ReadSheet<FooBar>();
}
```


## RoughlySQLite

RoughlySQLiteはCREATE TABLEとINSERTだけに特化したSQLiteのライブラリです。
（将来的にはその他の機能に対応するかもしれません。）

Crocellと同様にテーブルに対応するクラスを定義します。

```
class FooBarTable
{
      [PrimaryKey]
          public int ID { get; set; }

              public string Name { get; set; }
}
```

そして、テーブルを生成して、データをinsertします。

```
var provider = new SQLiteConnectionProvider("foobar.sqlite");
using(var connection = provider.GetOpenConnection())
{
      connection.CreateTable<FooBarTable>());

          connection.Insert<FooBarTable>(new FooBarTable { ID = 0, Name = "foo" });
}
```

これによりSQLiteのデータベースを簡単に生成することができます。

## Buffetについて

BuffetはCrocellによりExcelシートからデータを取得、それをSQLiteデータベースに取り込むことで、Excelでは管理しにくいシートやファイルをまたいだ横断的なデータの検索を効率的に行えるようになることを目的としています。

各ライブラリの詳細な使用方法はCrocellTestやRoughlySQLiteTestのプロジェクトに含まれているテストコードを参照してください。
