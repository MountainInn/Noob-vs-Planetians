using UnityEngine;

public class Volume
{
    public float current, maximum;

    public float Unfilled => maximum - current;


    public Volume() {}
    public Volume(float currentAndMaximum)
        :this(currentAndMaximum, currentAndMaximum)
    {

    }

    public Volume(float current, float maximum)
    {
        this.current = current;
        this.maximum = maximum;
    }

    public bool IsFull => (current == maximum);
    public bool IsEmpty => (current == 0);

    public void ResetToZero()
    {
        current = 0;
    }

    public float Ratio
    {
        get {
            float ratio = (current / maximum);

            if (float.IsNaN(ratio))
                ratio = 0;

            return ratio;
        }
    }

    public void ResetTo(float newCurrentAmount)
    {
        current = newCurrentAmount;
    }

    public bool Tick()
    {
        bool result = Add(Time.deltaTime);

        if (result)
            current = 0;

        return result;
    }

    public bool Add(float amount, out float overflow)
    {
        overflow = 0;

        if (amount > Unfilled)
            overflow = amount - Unfilled;

        return Add(amount);
    }
    public bool Add(float amount)
    {
        amount = Mathf.Min(amount, Unfilled);
        current = Mathf.Clamp(current + amount,
                              0, maximum);

        return current >= maximum;
    }

    public bool Subtract(float amount, out float overflow)
    {
        overflow = 0;

        if (amount > current)
            overflow = amount - current;

        return Subtract(amount);
    }

    public bool Subtract(float amount)
    {
        current -= Mathf.Min(amount, current);

        return current <= 0;
    }

    public void Reinitialize(float current, float maximum)
    {
        this.current = current;
        this.maximum = maximum;
    }

    public void ResizeAndRefill(float newMaximum)
    {
        maximum = newMaximum;
        current = newMaximum;
    }

    public void Resize(float newMaximum)
    {
        maximum = newMaximum;
        current = Clamp(current);
    }

    public void Refill()
    {
        current = maximum;
    }

    float Clamp(float amount)
    {
        return Mathf.Clamp(amount, 0, maximum);
    }

    public override string ToString()
    {
        return $"{current:0}/{maximum:0}";
    }
}
