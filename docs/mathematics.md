# Математика, Формулы, Шизофрения

<img src="https://sun9-9.userapi.com/impg/TvxOs5Z6Oq4zIVtUnJD0uvbLUPHa86M0OkuSBQ/xwSvc-KOU-s.jpg?size=107x55&quality=96&sign=80e1a5000a20607c8bd1afe5453abefc&type=album" align="right"/>

#### [Наш сайт](https://in-case.games) | [API](https://api.in-case.games/api/) | [Документация](redirection.md)

> Коммерческий проект по открытию кейсов.</br>
> Кейсы могут содержать в себе любые предметы из
> списка подключенных игр.

<b>Напутствие:</b>

У каждого предмета в кейсе, есть своя цена и вероятность на выпадение.</br>
Вероятность находится, по математической формуле весов.

<b>Обозначения:</b>

- chance - вероятность выпадения предмета</br>
- cost - стоимость предмета</br>
- wh - вес предмета или 1/cost</br>
- whAll - вес всех предметов или (1/cost1) + ... + (1/costn)</br>

<b>Формулы:</b>

- Сокращенный вид: <b>chance = wh/whAll</b>
- Полный вид: <b>chance = (1/cost1)/((1/cost1) + ... + (1/costn))</b>
