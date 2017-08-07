namespace App.Web.Helper
{
    public static class Toastr
    {
        public static string Added
        {
            get { return "toastr.success('Information successfully Added!', 'Success!');"; }
        }
        public static string Updated
        {
            get { return "toastr.success('Information successfully Updated!', 'Success!');"; }
        }
        public static string Deleted
        {
            get { return "toastr.success('Information successfully Deleted!', 'Success!');"; }
        }

        public static string BadRequest
        {
            get { return "toastr.error('Bad request!', 'Error!');"; }
        }

        public static string HttpNotFound
        {
            get { return "toastr.error('Information not found!', 'Error!');"; }
        }

        public static string DbError(string exceptionMessage)
        {
            return !string.IsNullOrWhiteSpace(exceptionMessage) ? 
                string.Format("toastr.error('{0}', 'Database Error!');", exceptionMessage) : 
                "toastr.error('Error occured in database, Try again!', 'Database Error!');";
        }
    }
}