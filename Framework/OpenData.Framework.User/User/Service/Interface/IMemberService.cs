using OpenData.Framework.Entity;
namespace OpenData.Framework.Core
{
    public interface IMemberService
    {
        void CreateAccount(string email, string phoneNumber, OpenData.Data.DynamicEntity data);
        void Import(System.Data.DataSet ds,  Site site);
        //Bzway.Business.Model.MemberSearchViewModel SearchMember(Bzway.Business.Model.MemberSearchViewModel model);
    }
}
