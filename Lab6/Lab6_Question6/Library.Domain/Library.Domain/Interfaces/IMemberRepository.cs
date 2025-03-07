using Library.Domain.Entities;

namespace Library.Domain.Interfaces
{
    public interface IMemberRepository
    {
        IEnumerable<Member> GetAllMembers();
        Member GetMemberById(int id);
        void AddMember(Member member);
        void UpdateMember(Member member);
        void DeleteMember(int id);
    }
}