using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// contrato minimo para los estados de la maquina de estados
public interface IState
{
    void Enter(); // llama una vez al estado 
    void Tick(float deltaTime); // llama cada frame 
    void Exit(); // llama una vez al salir del estado
}

