using System;

namespace Neft.Aggregation
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
    public class ExcelColumnHeaderAttribute : Attribute
    {
        // see the attribute guidelines at 
        //  http://go.microsoft.com/fwlink/?linkid=85236
        public readonly string columnName;

        // this is a positional argument
        public ExcelColumnHeaderAttribute(string columnName)
        {
            this.columnName = columnName;
        }
    }
}
