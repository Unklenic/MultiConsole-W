using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WccPcm.DataProvider
{
    public enum ElementType
    {
        /// <summary>
        ///  Набор битов.
        /// </summary>
        DPEL_BIT32 = 24,

        /// <summary>
        ///  Структура битового шаблона.
        /// </summary>
        DPEL_BIT32_STRUCT = 16,

        /// <summary>
        ///  Blob (большой двоичный объект).
        /// </summary>
        DPEL_BLOB = 46,

        /// <summary>
        ///  Структура большого двоичного объекта.
        /// </summary>
        DPEL_BLOB_STRUCT = 47,

        /// <summary>
        ///  Бит.
        /// </summary>
        DPEL_BOOL = 23,

        /// <summary>
        ///  Структура бита.
        /// </summary>
        DPEL_BOOL_STRUCT = 15,

        /// <summary>
        ///  Символ.
        /// </summary>
        DPEL_CHAR = 19,

        /// <summary>
        ///  Структура символа.
        /// </summary>
        DPEL_CHAR_STRUCT = 11,

        /// <summary>
        ///  Идентификатор.
        /// </summary>
        DPEL_DPID = 27,

        /// <summary>
        ///  Структура идентификатора.
        /// </summary>
        DPEL_DPID_STRUCT = 39,

        /// <summary>
        ///  Динамическое поле битового шаблона.
        /// </summary>
        DPEL_DYN_BIT32 = 8,

        /// <summary>
        ///  Структура динамического поля битового шаблона.
        /// </summary>
        DPEL_DYN_BIT32_STRUCT = 35,

        /// <summary>
        ///  Динамический большой двоичный объект.
        /// </summary>
        DPEL_DYN_BLOB = 48,

        /// <summary>
        ///  Динамическая структура большого двоичного объекта.
        /// </summary>
        DPEL_DYN_BLOB_STRUCT = 49,

        /// <summary>
        ///  Динамическое поле большого двоичного объекта.
        /// </summary>
        DPEL_DYN_BOOL = 7,

        /// <summary>
        ///  Динамическая структура поля бита.
        /// </summary>
        DPEL_DYN_BOOL_STRUCT = 34,

        /// <summary>
        ///  Динамическое поле символа.
        /// </summary>
        DPEL_DYN_CHAR = 3,

        /// <summary>
        ///  Динамическая структура поля символа.
        /// </summary>
        DPEL_DYN_CHAR_STRUCT = 30,

        /// <summary>
        ///  Динамическое поле идентификатора.
        /// </summary>
        DPEL_DYN_DPID = 29,

        /// <summary>
        ///  Динамическая структура поля идентификатора.
        /// </summary>
        DPEL_DYN_DPID_STRUCT = 38,

        /// <summary>
        ///  Динамическое поле с плавающей запятой.
        /// </summary>
        DPEL_DYN_FLOAT = 6,

        /// <summary>
        ///  Динамическая структура числа с плавающей запятой.
        /// </summary>
        DPEL_DYN_FLOAT_STRUCT = 33,

        /// <summary>
        ///  Динамическое поле целого числа.
        /// </summary>
        DPEL_DYN_INT = 5,

        /// <summary>
        ///  Динамическая структура целого числа.
        /// </summary>
        DPEL_DYN_INT_STRUCT = 32,

        /// <summary>
        ///  Многоязычное динамическое текстовое поле.
        /// </summary>
        DPEL_DYN_LANGSTRING = 44,

        /// <summary>
        ///  Cтруктура многоязычного динамического массива текстов.
        /// </summary>
        DPEL_DYN_LANGSTRING_STRUCT = 45,

        /// <summary>
        ///  Динамическое текстовое поле.
        /// </summary>
        DPEL_DYN_STRING = 9,

        /// <summary>
        ///  Динамическая структура текстового поля.
        /// </summary>
        DPEL_DYN_STRING_STRUCT = 36,

        /// <summary>
        ///  Динамическое поле времени.
        /// </summary>
        DPEL_DYN_TIME = 10,

        /// <summary>
        ///  Динамическая структура поля времени.
        /// </summary>
        DPEL_DYN_TIME_STRUCT = 37,

        /// <summary>
        ///  Динамическое поле положительных целых чисел.
        /// </summary>
        DPEL_DYN_UINT = 4,

        /// <summary>
        ///  Динамическая структура натуральных чисел.
        /// </summary>
        DPEL_DYN_UINT_STRUCT = 31,

        /// <summary>
        ///  Число с плавающей точкой.
        /// </summary>
        DPEL_FLOAT = 22,

        /// <summary>
        ///  Структура числа с плавающей запятой.
        /// </summary>
        DPEL_FLOAT_STRUCT = 14,

        /// <summary>
        ///  Целое число.
        /// </summary>
        DPEL_INT = 21,

        /// <summary>
        ///  Целочисленная структура.
        /// </summary>
        DPEL_INT_STRUCT = 13,

        /// <summary>
        ///  Описание.
        /// </summary>
        DPEL_LANGSTRING = 42,

        /// <summary>
        ///  Cтруктура описания.
        /// </summary>
        DPEL_LANGSTRING_STRUCT = 43,

        /// <summary>
        ///  Текст.
        /// </summary>
        DPEL_STRING = 25,

        /// <summary>
        ///  Структура текста.
        /// </summary>
        DPEL_STRING_STRUCT = 17,

        /// <summary>
        ///  Cтруктура.
        /// </summary>
        DPEL_STRUCT = 1,

        /// <summary>
        ///  Time.
        /// </summary>
        DPEL_TIME = 26,

        /// <summary>
        ///  Cтруктура обозначений времени.
        /// </summary>
        DPEL_TIME_STRUCT = 18,

        /// <summary>
        ///  Ссылка на точку данных.
        /// </summary>
        DPEL_TYPEREF = 41,

        /// <summary>
        ///  Натуральное число.
        /// </summary>
        DPEL_UINT = 20,

        /// <summary>
        ///  Структура натуральных чисел.
        /// </summary>
        DPEL_UINT_STRUCT = 12,

        /// <summary>
        ///  Пустой тип.
        /// </summary>
        DPEL_NONE = 0
    }
}
