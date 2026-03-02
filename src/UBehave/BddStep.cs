namespace UBehave;

public record struct BddStep(string Step);

public record struct BddStep<T>(string Step, T Result);
