namespace Novolis.Game.MenuFlows;

/// <summary>Records a navigation from one screen to another.</summary>
/// <param name="FromScreenId">Previous screen id, if any.</param>
/// <param name="ToScreenId">New active screen id.</param>
public readonly record struct GameScreenTransition(string FromScreenId, string ToScreenId);
