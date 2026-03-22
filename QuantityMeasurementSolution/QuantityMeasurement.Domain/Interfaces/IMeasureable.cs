namespace QuantityMeasurement.Domain.Interfaces
{
    /// <summary>
    /// Interface for measurable quantities, defining methods for unit conversion and retrieval of unit name. 
    /// This abstraction allows for consistent handling of different quantity types (e.g., length, weight) and adheres to the Open/Closed principle by enabling new quantity types to implement this interface without modifying existing code.
    /// </summary>
    public interface IMeasurable
    {
        // Returns the conversion factor to the base unit (e.g., feet for length).
        double GetConversionFactor();
        
        // Converts a value to the base unit using the conversion factor.
        double ConvertToBaseUnit(double value);
        
        // Converts a value from the base unit to the target unit using the conversion factor.
        double ConvertFromBaseUnit(double baseValue);

        // Returns the name of the unit as a string, useful for display purposes.
        string GetUnitName();
    }
}