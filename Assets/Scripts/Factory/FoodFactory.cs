using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodFactory : ABaseFactory<IFood>
{
    public override IFood Create(object arg = null)
    {
        if (arg is FoodView foodViewPfb)
        {
            var foodView = MonoBehaviour.Instantiate(foodViewPfb);
            IFood food = new SimpleFood(foodView);
            return food;
        }
        else return null;
    }
}
