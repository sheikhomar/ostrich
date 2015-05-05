using System;

namespace ConsoleUI
{
    public class RecordNotFoundException : Exception
    {
        public RecordNotFoundException(string type, int id) :
            base(String.Format("{0} with the ID {1} was not found.", type, id))
        {
            
        }
    }
}