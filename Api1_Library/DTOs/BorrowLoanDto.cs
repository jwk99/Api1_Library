namespace Api1_Library.DTOs
{

        public record BorrowLoanDto(int MemberId, int BookId, int Days = 14);
}
