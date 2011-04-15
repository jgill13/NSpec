using System;
using System.Collections.Generic;
using System.Linq;
using NSpec;

namespace SampleSpecs.Compare.NSpec
{
    class VendingMachineSpec : nspec
    {
        void given_new_vending_machine()
        {
            before = () => machine = new VendingMachine();

            specify = ()=> machine.Items().should_be_empty();

            it["getting item A1 should throw ItemNotRegistered"] = expect<ItemNotRegisteredException>(() => machine.Item("A1"));

            context["given doritos are registered in A1 for 50 cents"] = () =>
            {
                before = () => machine.RegisterItem("A1", "doritos", .5m);

                specify = () => machine.Items().Count().should_be(1);

                specify = () => machine.Item("A1").Name.should_be("doritos");

                specify = () => machine.Item("A1").Price.should_be(.5m);
            };
        }
        private VendingMachine machine;
    }

    public class ItemNotRegisteredException : Exception
    {
    }

    internal class VendingMachine
    {
        public VendingMachine()
        {
            items = new Item[] { };
        }

        public IEnumerable<Item> Items()
        {
            return items;
        }

        public void RegisterItem(string slot, string name, decimal price)
        {
            items = new[]{new Item{Name = name,Price = price}};
        }

        public Item Item(string slot)
        {
            if(!items.Any(i=> i.Slot==slot))
            return items.First();
        }
        private Item[] items;
    }

    internal class Item
    {
        public string Name { get; set; }

        public decimal Price { get; set; }

        public string Slot { get; set; }
    }
}