using OpenData.Site.Entity;
namespace OpenData.Site.Core
{
    public interface IMemberService
    {
        void CreateAccount(string email, string phoneNumber, OpenData.Data.DynamicEntity data);
        void Import(System.Data.DataSet ds, Entity.Site site);
        //Bzway.Business.Model.MemberSearchViewModel SearchMember(Bzway.Business.Model.MemberSearchViewModel model);
    }
}
