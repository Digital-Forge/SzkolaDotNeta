using ConsoleToDoList.ConsoleTerminal;
using System;

namespace ConsoleToDoList.App
{
    public class DateAdapter : IConsoleReadable
    {
        private DateTime? _date;

        public DateTime? Date { get => _date; private set { } }

        public int? Year 
        { 
            get => _date?.Year; 
            set
            {
                if (value != null)
                {
                    try
                    {
                        if (_date == null) _date = new DateTime(value.Value, 1, 1);
                        else               _date = new DateTime(value.Value, _date.Value.Month, _date.Value.Day);
                    }
                    catch
                    {}
                } 
            }
        }

        public int? Month
        {
            get => _date?.Month;
            set
            {
                if (value != null)
                {
                    try
                    {
                        if (_date == null)
                        {
                            if (value.Value < DateTime.Now.Month)  _date = new DateTime(DateTime.Now.Year + 1, value.Value, 1);
                            if (value.Value == DateTime.Now.Month) _date = new DateTime(DateTime.Now.Year, value.Value, DateTime.Now.Day + 1);
                            else                                   _date = new DateTime(DateTime.Now.Year, value.Value, 1);

                        }
                        else
                        {
                            _date = new DateTime(_date.Value.Year ,value.Value, _date.Value.Day);
                        }
                    }
                    catch
                    { }
                }
            }
        }

        public int? Day
        {
            get => _date?.Day;
            set
            {
                if (value != null)
                {
                    try
                    {
                        if (_date == null)
                        {
                            if (value.Value <= DateTime.Now.Day)
                            {
                                if (DateTime.Now.Month == 12) _date = new DateTime(DateTime.Now.Year + 1, 1, value.Value);
                                else                          _date = new DateTime(DateTime.Now.Year, DateTime.Now.Month + 1, value.Value);
                            }
                            else _date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, value.Value);

                        }
                        else
                        {
                            _date = new DateTime(_date.Value.Year, value.Value, _date.Value.Day);
                        }
                    }
                    catch
                    { }
                }
            }
        }

        public DateAdapter(DateTime? date = null)
        {
            _date = date;
        }
    }
}
