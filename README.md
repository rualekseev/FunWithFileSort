# FunWithFileSort
Генерация и сортировка файла

Саму задачу тут описывать не буду, так как хочу решение оставить в публичном репозитории, и чтобы по ключевым словам задача не находилась.

Предположение в рамках которых веду разработку:
* Размерность Number не указана, поэтому не буду парсить в какие то числовые значения, а буду сравнивать и хранить как строки.
* Если встречу в потоке строчку с неправильным форматом, буду падать
* Соответвенно не дам сгенерировать файл меньше 3х символов. Минимальная строка [цифра][точка][пробел]
* Против написания тестов ничего не имею, но так как это тестовое задание, просто покажу что я их умею писать. Сильно много времени тартить не буду.
* Учитывая, что данных много, то надо выбрать алгоритм сортировки который может работать в несколько потоков.
* Исхожу из того, что строчка в файле разумных размеров, то есть на вход не подастся файл в 100Gb состощий из оной строки. В общем в памяти должно хватать места для нескольких строк.

## Генерация файла
1. Есть обьект который умеет генерировать строки по заданному шаблону.
2. Есть обьект который используя генератор строк, умеет генерировать текст. Включая в текст повторение строк
3. Есть консольное приложение которое умеет генерировать текст в файл(windows style), или просто в out (linux style)

## Сортировка файла
Так как файл большой и в память его засовывать не будем, я бы рассмотрел 2 алгоритма для реализации (если бы имел доступ к уточнениям по заданию).
1. Сортировка выбором или сортировку вставками. Медленный алгоритм, но почти не требует затрат по памяти. Фактически мы много раз читаем первый файл, а созданный файл у нас всегда отсортированный и растёт пока не достигнет размеров читаемого файла.
2. Быстрая сортировка или сортировка слиянием. Преимущества над 1. можно паралелить, более быстрый. Из минусов создание примерно log(n) файлов, где n это количество строк. Так как для быстрой сортировки не понятно как качественно выбрать разделяющий элемент, скорее всего буду делать сортировку слиянием.


# Результат
## Создание файла
Стэнд Ubuntu 20.04 x64 8Gb/4CPU/160Gb SSD

Генерация файла linux утилитой
```dd if=/dev/urandom of=result bs=1k count=10485760 seek=0```
10737418240 bytes (11 GB, 10 GiB) copied, 93.0978 s, 115 MB/s
                                          
```dd if=/dev/urandom of=result bs=512 count=20971520 seek=0```
10737418240 bytes (11 GB, 10 GiB) copied, 119.576 s, 89.8 MB/s

```dd if=/dev/urandom of=result bs=64 count=167772160 seek=0```
10737418240 bytes (11 GB, 10 GiB) copied, 495.028 s, 21.7 MB/s

Генерация файла быстрым алгоритмом (это повторяющаяся одна и та же строка)
```./FileGenerator -size=10737418240 -alg=fast >> result```
Elapsed 499 seconds


Генерация файла случайными строками без контролируемых повторений
```./FileGenerator -size=10737418240 -alg=rand >> result```
Elapsed 1267 seconds

Генерация файла случайными строками без контролируемых повторений (оптимизация по Empty обьектам)
```./FileGenerator -size=10737418240 -alg=rand >> result```
Elapsed 1181 seconds

Генерация файла случайными строками без контролируемых повторений (кэширую по ~1024 байта перед выводом)
```./FileGenerator -size=10737418240 -alg=rand >> result```
Elapsed 439 seconds
(!) быстрый алгоритм с записью по ~1024 байта отработал за 138 seconds. 
Вывод, оптимизировать создание строки, по записи на диск мы уже добились нужных значений.

Кэшируем вызов Encoding.GetByteCount
Elapsed 427 seconds

10Gb генерируем за 361 сек

План
- Оптимизировать сборку мусора (переделать на работу с char)
- Добавить алгоритм с повторениями значений после точки. на вход X% (значение от 1 до 100). Сам алгоритм с вероятностью X запоминает значение, с вероятностью X дубдлирует запомненое значение. Под значением подразумевается его первая часть до .



## Сортировка файла
Накидал драфт для сортировки методом InsertionSort и MergeSort
MergeSort пока работает медленно так как мержит узлы дерева разного ранга

План:
- ~~сделать для MergeSort сбланасированное дерево~~
- ~~При MergeSort использовать для файлов меньше 1Мб (или 100 Кб) алгоритм InsertionSort. То есть не уходить в файлы длинной 1 строка~~
- ~~Сделать MergeSort многопоточным.~~
- ~~Перевести сравнение на сравнение по алгоритму, а не по строке~~
- Убрать лишний пробел при материлизации из line в Row и обратно

По итогу, отказался от сортировки через InsertionSort для маленьких файлов так как всё равно медленно, и маленькие файлы сортирую в памяти.
Колчество потоков на шаг алгортма сейчас константа и прописан в коде.
Результат тестирования
100mb отсортировно за XXX сек(файлы 16Mb сортируются в памяти)
2Gb отсортировно за XXX сек (файлы 25Kb сортируются в памяти)
10Gb отсортировно за XXX сек (файлы 25Kgitcd b сортируются в памяти)

