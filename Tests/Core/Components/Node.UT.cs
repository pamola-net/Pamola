﻿using System;
using System.Linq;
using Xunit;
using System.Numerics;

namespace Pamola.UT
{
    public class NodeUT
    {
        /// <summary>
        /// Checks if <see cref="Pamola.Node.AdjacentComponents"/> list matches the <see cref="Pamola.Node.Terminals"/> list.
        /// </summary>
        [Fact]
        public void AdjacentComponentsMatchTerminals()
        {
            var element = new MockedElement(2);
            var node = element.Terminals.First().ConnectTo(element.Terminals.Last());

            IComponent component = node;
            Assert.Equal(component.AdjacentComponents, node.Terminals);
        }

        [Fact]
        public void NodeVariablesHasOnlyOneVariable()
        {
            IComponent node = new MockedElement(2).Terminals.ConnectAll();
            Assert.Single(node.Variables);
        }

        [Fact]
        public void NodeVariablesSetsCurrent()
        {
            var node = new MockedElement(2).Terminals.ConnectAll();
            IComponent component = node;
            var voltage = 3.0;

            component.Variables.First().Setter(voltage);

            Assert.Equal(voltage, node.Voltage);
        }

        [Fact]
        public void NodeObeysKirchhoffsCurrentLaw()
        {
            var node = new MockedElement(3).Terminals.ConnectAll();

            node.Terminals.Cast<IComponent>().SelectMany(t => t.Variables).ToList().ForEach(t => t.Setter(1.0));
            var equation = ((IComponent)node).Equations.First();

            Assert.Equal(3.0, equation());
        }
    }
}
