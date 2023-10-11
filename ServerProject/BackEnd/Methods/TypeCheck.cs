
namespace IS_Arch.BackEnd
{
    internal class TypeCheck
    {
        public static bool BoolCheck(string sendedbool)
        {
            bool varbool;
            try
            {
                varbool = bool.Parse(sendedbool);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool IntCheckBool(string numb)
        {
            int varint;
            try
            {
                varint = int.Parse(numb);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
