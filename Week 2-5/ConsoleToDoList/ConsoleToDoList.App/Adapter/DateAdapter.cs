using ConsoleToDoList.ConsoleTerminal;
using System;

namespace ConsoleToDoList.App
{
    public class DateAdapter : IConsoleReadable
    {
        public DateTime? Date;

        public int? Year 
        { 
            get => Date?.Year; 
            set
            {
                if (value != null)
                {
                    try
                    {
                        if (Date == null) Date = new DateTime(value.Value, 1, 1);
                        else              Date = new DateTime(value.Value, Date.Value.Month, Date.Value.Day);
                    }
                    catch
                    {}
                } 
            }
        }

        public int? Month
        {
            get => Date?.Month;
            set
            {
                if (value != null)
                {
                    try
                    {
                        if (Date == null)
                        {
                            if (value.Value < DateTime.Now.Month)  Date = new DateTime(DateTime.Now.Year + 1, value.Value, 1);
                            if (value.Value == DateTime.Now.Month) Date = new DateTime(DateTime.Now.Year, value.Value, DateTime.Now.Day + 1);
                            else                                   Date = new DateTime(DateTime.Now.Year, value.Value, 1);

                        }
                        else
                        {
                            Date = new DateTime(Date.Value.Year ,value.Value, Date.Value.Day);
                        }
                    }
                    catch
                    { }
                }
            }
        }

        public int? Day
        {
            get => Date?.Day;
            set
            {
                if (value != null)
                {
                    try
                    {
                        if (Date == null)
                        {
                            if (value.Value <= DateTime.Now.Day)
                            {
                                if (DateTime.Now.Month == 12) Date = new DateTime(DateTime.Now.Year + 1, 1, value.Value);
                                else                          Date = new DateTime(DateTime.Now.Year, DateTime.Now.Month + 1, value.Value);
                            }
                            else Date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, value.Value);

                        }
                        else
                        {
                            Date = new DateTime(Date.Value.Year, Date.Value.Month, value.Value);
                        }
                    }
                    catch
                    { }
                }
            }
        }

        public DateAdapter(DateTime? date = null)
        {
            Date = date;
        }
    }
}
