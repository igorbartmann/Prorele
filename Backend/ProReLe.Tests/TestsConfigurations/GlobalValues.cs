namespace ProReLe.Tests
{
    public class GlobalValues
    {
        // Database.
        public const string CONNECTION_STRING = "Data Source=localhost;Initial Catalog=ProReLeDatabase;User ID=sa;Password=gvdasa;Persist Security Info=True;MultipleActiveResultSets=True";

        // Product.
        public const string PRODUCT_DESCRIPTION_BEFORE_EDIT = "Product To Test Added";
        public const string PRODUCT_DESCRIPTION_AFTER_EDIT = "Product To Test Edited";
        public const decimal PRODUCT_PRICE_BEFORE_EDIT = 25.00M;
        public const decimal PRODUCT_PRICE_AFTER_EDIT = 26.75M;
        public const int PRODUCT_AMOUNT_BEFORE_EDIT = 10;
        public const int PRODUCT_AMOUNT_AFTER_EDIT = 8;

        // Client.
        public const string CLIENT_CPF = "04598745082";
        public const string CLIENT_NAME_BEFORE_EDIT = "Musterman Um";
        public const string CLIENT_NAME_AFTER_EDIT = "Musterman Dois";

        // Sale.
        public const string SALE_PRODUCT_DESCRIPTION = "Product to Sale";
        public const decimal SALE_PRODUCT_PRICE = 58.75M;
        public const int SALE_PRODUCT_AMOUNT = 8;
        public const string SALE_CLIENT_NAME = "Client of Sale";
        public const string SALE_CLIENT_CPF = "04065276010";
        public const int SALE_AMOUNT = 2;
        public const decimal SALE_DISCONT_BEFORE_EDIT = 4.5M;
        public const decimal SALE_DISCOUNT_AFTER_EDIT = 10M;
    }
}