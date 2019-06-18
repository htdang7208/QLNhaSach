namespace QLNhaSach.Utils
{
    public class Roles
    {
        public static int MaxBookStock = 300;
        public static int MinBookInput = 150;
        public static bool GetOverDept = true;
        public static double DeptOver = 20000;
        public static int StockMax = 20;

        public static int NotFound = 404;
        public static int Success = 200;
        public static int Existed_Username = 601;
        public static int Existed_Phone = 602;
        public static int Empty = 603;
        public static int Password_Not_Match_Origin = 604;
        public static int Password_Not_Match_Confirm = 605;

        public static int ErrorGet = 1;
        public static int ErrorPost = 2;
        public static int ErrorPut = 3;
        public static int ErrorDelete = 4;
        public static int SyntaxEmail = 5;   // syntax error
        public static int EmptyUsernamePassword = 7;// username has existed
        public static int Removed = 8;      // is removed

        public static int OverflowMaxStock = 9;
        public static int NotEnoughMinStock = 10;

        public static int Empty_Book_Name = 11;
        public static int Empty_Book_Price = 12;
        public static int Empty_Book_Kind = 13;
        public static int Empty_Book_Author = 14;
        public static int Empty_Book_Stock = 15;
        public static int Existed_Book = 16;
        public static int Book_Removed = 18;

        public static int Empty_Receipt = 19;
        public static int Get_Over_Dept = 20;

        public static int Empty_Customer_Input = 21;
        public static int Empty_Customer_FirstName = 22;
        public static int Empty_Customer_LastName = 23;
        public static int Empty_Customer_Total = 24;
        public static int Empty_Customer_Paid = 25;

        
    }
}
