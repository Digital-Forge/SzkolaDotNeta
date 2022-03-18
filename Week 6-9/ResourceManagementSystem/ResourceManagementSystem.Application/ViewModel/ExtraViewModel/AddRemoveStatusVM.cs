using System;
using System.Collections.Generic;
using System.Text;

namespace ResourceManagementSystem.Application.ViewModel.ExtraViewModel
{
    public class AddRemoveStatusVM
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool Status { get; set; } = false;
    }
}
