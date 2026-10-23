/*

 Author: Marcos Vizio
 Email: marcosfvizio@gmail.com

 Creation Date: 2026-10-22 16:31:27
 Last Modification Date: 2026-10-23 00:31:00

*/
public interface IHealth
{
    float CurrentHealth { get; }
    float MaxHealth { get; }
    event System.Action<float, float> HealthChanged;
}
