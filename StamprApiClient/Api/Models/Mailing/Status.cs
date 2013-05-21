using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StamprApiClient.Api.Models.Mailing
{
    public enum Status
    {
        received,
        render,
        error,
        queued,
        assigned,
        processing,
        printed,
        shipped
    }
}
