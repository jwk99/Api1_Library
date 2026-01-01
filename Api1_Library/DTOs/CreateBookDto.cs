namespace Api1_Library.DTOs
{
    public record CreateBookDto(string Title, string Author, int Copies = 1);
}
