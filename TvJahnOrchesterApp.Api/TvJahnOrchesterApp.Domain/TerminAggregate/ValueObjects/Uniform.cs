using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TvJahnOrchesterApp.Domain.Common.Models;

namespace TvJahnOrchesterApp.Domain.TerminAggregate.ValueObjects
{

    public sealed class Uniform : ValueObject
    {
        public UniformEnum UniformEnum { get; set; }

        private Uniform() { }

        private Uniform(UniformEnum uniformEnum)
        {
            UniformEnum = uniformEnum;
        }

        public override IEnumerable<object> GetEqualityComponents()
        {
            yield return UniformEnum;
        }

        public static Uniform Create(UniformEnum uniformEnum)
        {
            return new Uniform(uniformEnum);
        }
    }

    public enum UniformEnum
    {
        BlauesHemd,
        WeißeHose,
        Jacket,
        WinterJacke,
        Zivil
    }
}
