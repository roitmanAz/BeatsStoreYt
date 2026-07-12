namespace BeatsStoreYt.API.DTOs.Cart;

public class AddBeatSetToCartRequestDto
{
    public int BeatSetId { get; set; }
    public int Quantity { get; set; } = 1;
}
