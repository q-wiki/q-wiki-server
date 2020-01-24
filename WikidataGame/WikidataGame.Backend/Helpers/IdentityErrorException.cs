using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WikidataGame.Backend.Helpers
{
    public class IdentityErrorException : Exception
    {
        public IdentityErrorException(IEnumerable<IdentityError> errors)
        {
            Errors = errors;
        }

        public IEnumerable<IdentityError> Errors { get; }

        public override string ToString()
        {
            return Errors.Select(e => e.Description).Aggregate((i, j) => $"{i}{Environment.NewLine}{j}");
        }
    }
}
