using System.Collections.Generic;

public class SpecBase
{
    private Queue<Spec> specs;

    public SpecBase()
    {
        specs = new Queue<Spec>();
    }

    public static SpecBase GetInstance()
    {
        return SpecBaseHolder.Instance;
    }

    public void PutSpec(Spec newSpec)
    {
        specs.Enqueue(newSpec);
    }

    public Spec TakeSpec()
    {
        return specs.Dequeue();
    }
    
    public static class SpecBaseHolder
    {
        public static SpecBase Instance;
    }
}