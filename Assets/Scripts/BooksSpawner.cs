using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BooksSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] books;
    private GameManager gameManager;
    private List<int> selectedIndices = new List<int>();
    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.Find("GameManager"))
        {
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        }

        ActivateBooksWithoutRepetition();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ActivateBooksWithoutRepetition()
    {
        int booksToActivate = gameManager.instance.booksToBeFound;
        int booksAvailable = books.Length;

        // Verifica si hay suficientes libros para activar sin repetición.
        if (booksToActivate > booksAvailable)
        {
            Debug.LogError("No hay suficientes libros disponibles para activar sin repetición.");
            return;
        }

        for (int i = 0; i < booksToActivate; i++)
        {
            int randomIndex = GetUniqueRandomIndex();

            // Verifica si el índice es válido.
            if (randomIndex != -1)
            {
                books[randomIndex].SetActive(true);
            }
            else
            {
                Debug.LogError("No se pudo encontrar un índice único para activar un libro.");
                return;
            }
        }
    }

    int GetUniqueRandomIndex()
    {
        int randomIndex = Random.Range(0, books.Length);

        // Verifica si el índice ya ha sido seleccionado.
        while (selectedIndices.Contains(randomIndex))
        {
            // Si el índice ya ha sido seleccionado, obtén uno nuevo.
            randomIndex = Random.Range(0, books.Length);

            // Si todos los libros ya se han seleccionado, retorna -1.
            if (selectedIndices.Count >= books.Length)
            {
                Debug.LogWarning("Todos los libros ya han sido seleccionados.");
                return -1;
            }
        }

        // Agrega el índice seleccionado a la lista de índices seleccionados.
        selectedIndices.Add(randomIndex);

        return randomIndex;
    }
}
