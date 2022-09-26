/// ---------------------------------------------
/// Contact: Henry Braun
/// Brief: Defines an Cell
/// Thanks to VHLab for original implementation
/// Date: November 2017 
/// ---------------------------------------------

using UnityEngine;
using System.Collections.Generic;

namespace Biocrowds.Core
{
    public class Cell : MonoBehaviour
    {
        public int X;
        public int Z;

        private List<Auxin> _auxins = new List<Auxin>();
        public int count;
        [SerializeField]
        private MeshRenderer _meshRenderer;

        public List<Auxin> Auxins
        {
            get { return _auxins; }
            set { _auxins = value; }
        }

        public void ShowMesh(bool _show)
        {
            _meshRenderer.enabled = _show;
        }

        private void Update()
        {
            count = _auxins.Count;
        }
    }
}