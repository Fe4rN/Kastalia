using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;

    private int totalEnemies = 0;
    private int enemiesRemaining = 0;

    public Victory_Manager victoryManager;

    private void Awake()
    {
        // se asegura de que solo haya una instancia
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        // Busca todos los objetos en la escena con el tag "Enemy"
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        // Cuenta los enemigos encontrados
        totalEnemies = enemies.Length;
        enemiesRemaining = totalEnemies;

        Debug.Log($"Enemigos encontrados al inicio: {totalEnemies}");

    }

    // Este método se puede seguir usando si se generan enemigos después del inicio
    public void RegisterEnemy()
    {
        totalEnemies++;
        enemiesRemaining++;
        Debug.Log($"Enemigo registrado. Total: {totalEnemies}, Restantes: {enemiesRemaining}");
    }

    // Llama este método cuando un enemigo muere
    public void UnregisterEnemy()
    {
        Debug.Log("Victory Manager en EnemyManager: " + victoryManager);

        enemiesRemaining--;
        Debug.Log($"Enemigo eliminado. Restantes: {enemiesRemaining}");

        // Si no quedan enemigos, se gana el juego
        if (enemiesRemaining <= 0)
        {
            Debug.Log("¡VICTORIA! Todos los enemigos han muerto.");
            victoryManager.WinGame();
        }
    }
}
