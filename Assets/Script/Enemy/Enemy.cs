using UnityEngine;

namespace Script.Character.Enemy
{
    public class Enemy : MonoBehaviour, IEatable
    {
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
        
        public void Eaten()
        {
            Destroy(gameObject);
        }
    }
}
