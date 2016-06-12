using OpenData.Framework.Entity;
using System;
namespace OpenData.Framework.Service
{
    public interface IMemberService
    {
        void CreateAccount(string email, string phoneNumber, OpenData.Data.DynamicEntity data);
        void Import(System.Data.DataSet ds,  Site site);
        //Bzway.Business.Model.MemberSearchViewModel SearchMember(Bzway.Business.Model.MemberSearchViewModel model);
    }
}
