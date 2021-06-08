using System;
using System.Collections.Generic;
using System.Numerics;

namespace Pamola.Transient
{
    /// <summary>
    /// Describes an <see cref="IdeaInductor"/> and its properties.
    /// </summary>
    public class IdealInductor : TransientDipole
    {
        /// <summary>
        /// Inductor's inductance, in Henry.
        /// </summary>
        /// <value><see cref="double"/>.</value>
        public double Inductance { get; set; }

        /// <summary>
        /// The induction's charge is the current flowing through its, 
        /// stored as a <see cref="double"/>.
        /// </summary>
        /// <value></value>
        public double Charge {get; set; }


        /// <summary>
        /// Creates a new <see cref="IdealInductor"/> with a given <paramref name="inductance"/>.
        /// </summary>
        /// <param name="inductance">Induction in Henry</param>
        public IdealInductor(double inductance)
        {
            Inductance = inductance;
            TransientVariables = new List<TransientVariable>()
            {
                new TransientVariable(new Variable(()=> Charge, x => Charge = x), InductorEquation)
            };
        }

        /// <summary>
        /// Overrides <see cref="TransientDipole.DipoleEquations"/> with a new list, 
        /// containg the <see cref="ChargingEquation"/>
        /// </summary>
        /// <returns>A read-only collection of functions of <see cref="double"/>.</returns>
        protected override IReadOnlyCollection<Func<double>> DipoleEquations => new List<Func<double>>() { ChargingEquation };

        /// <summary>
        /// Overrides <see cref="TransientDipole.TransientVariables"/>, with a list containg the <see cref="InductorEquation"/>.
        /// </summary>
        /// <value>A read-only <see cref="TransientVariable"/> collection.</value>
        protected override IReadOnlyCollection<TransientVariable> TransientVariables { get; }

        /// <summary>
        /// Overrides <see cref="Element.Variables"/> with an empty list.
        /// </summary>
        /// <typeparam name="Variable"></typeparam>
        /// <returns></returns>
        protected override IReadOnlyCollection<Variable> Variables => new List<Variable>();

        /// <summary>
        /// Represents the <see cref="IdealInductor"/> equation, in response to its charging equation.
        /// It's the value of the derivative of the current flowing in the inductor.
        /// </summary>
        /// <returns>The voltage divided by the <see cref="Inductance"/>.</returns>
        private double InductorEquation() => 
            (Positive.Node.Voltage - Negative.Node.Voltage) / Inductance;
        
        /// <summary>
        /// Represents the acutal derivative of the current flowing through the <see cref="IdealInductor"/>.
        /// </summary>
        /// <returns>The different between the current flowing in the <see cref="IdealInductor"/>,
        /// and its actual <see cref="Charge"/>.</returns>
        private double ChargingEquation() => Positive.Current - Charge;

    }
}