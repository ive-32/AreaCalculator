## Cтруктура БД
### Таблица Продуктов Products (Id, Name)

| Id | Name |
| :--- | :--- |
| 1 | Chair |
| 2 | Sofa |
| 3 | Drawer |
| 4 | Table |
| 5 | Bed |

### Таблица Категорий Category (Id, Name)

| Id | Name |
| :--- | :--- |
| 1 | Soft |
| 2 | Hard |
| 3 | Living Room |
| 4 | Kitchen |
| 5 | BathRoom |

### Таблица для связи многие ко многим PrdCatLinks (Id, ProdId, CatId)

| Id | ProdId | CatId |
| :--- |:------:|:-----:|
| 1 |   1    |   1   |
| 2 |   1    |   3   |
| 3 |   2    |   1   |
| 4 |   2    |   3   |
| 5 |   4    |   2   |
| 6 |   4    |   3   |
| 7 |   4    |   4   |
| 8 |   5    |   3   |

Не будем связывать Шкаф (3, Drawer) ни с какой категорией <br>
Не будем связывать категорию Ванная (5, BathRoom) ни с каким продуктом

## Запрос для выборки всех продуктов и категорий для них
включает продукты без категорий <br>
Отсортирован по имени продукта и затем по категории

```sql
select p.Name, c.Name 
    from Products P
    left join ProdCatLinks Links on p.Id = Links.ProdId
    left join Category C on Links.CatId = c.Id
order by p.Name, c.Name
```

| Name | Name |
| :--- | :--- |
| Bed | Living Room |
| Chair | Living Room |
| Chair | Soft |
| Drawer | null |
| Sofa | Living Room |
| Sofa | Soft |
| Table | Hard |
| Table | Kitchen |
| Table | Living Room |


## Запрос для выборки продуктов и категорий, где есть все продукты и все категории 

```sql
select p.Name, c.Name from Products P
   left join ProdCatLinks Links on p.Id = Links.ProdId
   full join dbo.Category C on Links.CatId = c.Id
order by p.Name, c.Name
```

| Name | Name |
| :--- | :--- |
| null | BathRoom |
| Bed | Living Room |
| Chair | Living Room |
| Chair | Soft |
| Drawer | null |
| Sofa | Living Room |
| Sofa | Soft |
| Table | Hard |
| Table | Kitchen |
| Table | Living Room |

## Запрос для выборки продуктов у которых больше одной категории 

```sql
select min(p.Name) as Name, count(Links.CatId) as Count 
    from Products P
    left join ProdCatLinks Links on p.Id = Links.ProdId
group by Links.ProdId
HAVING count(Links.CatId) > 1
```

| Name  | Count  |
|:------|:------:|
| Chair |   2    |
| Sofa  |   2    |
| Table |   3    |

## Запрос для поиска сирот в таблицах Product и Category

```sql
select 'Product', min(p.Name), count(Links.CatId) 
    from Products P
    left join ProdCatLinks Links on p.Id = Links.ProdId
group by Links.ProdId
HAVING count(Links.Id) = 0
union
select 'Category', min(c.Name), count(Links.ProdId) 
    from Category c
    left join ProdCatLinks Links on c.Id = Links.CatId
group by Links.CatId
HAVING count(Links.Id) = 0

```

|  |  |  |
| :--- | :--- | :--- |
| Category | BathRoom | 0 |
| Product | Drawer | 0 |

## Еще один запрос для поиска сирот

```sql
select p.Name, c.Name from Products P
                               left join ProdCatLinks Links on p.Id = Links.ProdId
                               full join dbo.Category C on Links.CatId = c.Id
group by p.Name, c.Name
having count(links.CatId) = 0 or count(links.ProdId) = 0
```

| Name | Name |
| :--- | :--- |
| Drawer | null |
| null | BathRoom |
