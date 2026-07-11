using System;

namespace task07
{
    [DisplayName("Демонстрационный Класс")]
    [Version("1.0")]
    public class SampleClass
    {
        [DisplayName("Идентификатор пользователя")]
        public int Id { get; set; }

        public string Name { get; set; }

        [DisplayName("Выполнить расчет")]
        public void Calculate()
        {
        }

        public void Reset()
        {
        }
    }
}