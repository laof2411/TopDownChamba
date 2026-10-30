using System.Collections;
using System.Linq;
using UnityEngine;

public class CycleManager : MonoBehaviour
{

    [SerializeField] private GameObject enemyPrefab;

    [SerializeField] private float timeBetweenDays;
    private Coroutine spawningCoroutine;

    [Header("Day Data")]
    [SerializeField] private float timeBetweenEnemy;
    [SerializeField] private Transform[] dayPositions;


    [Header("Night Data")]
    [SerializeField] private int enemyAmount = 0;
    [SerializeField] private Transform[] nightPositions;


    private void Awake()
    {
        StartCoroutine(Cycle());
    }

    private IEnumerator Cycle()
    {
        while (true)
        {
            if (spawningCoroutine != null)
            {
                StopCoroutine(spawningCoroutine);
            }

            spawningCoroutine = StartCoroutine(DayBehaviour());
            yield return new WaitForSeconds(timeBetweenDays);

            spawningCoroutine = StartCoroutine(NightBehaviour());
            yield return new WaitForSeconds(timeBetweenDays);
        }
    }

    private IEnumerator DayBehaviour()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeBetweenEnemy);
            int randomPosition = Random.Range(0, dayPositions.Count() - 1);
            Instantiate(enemyPrefab, dayPositions[randomPosition].position, Quaternion.identity);
        }
    }

    private IEnumerator NightBehaviour()
    {
        enemyAmount += 3;

        int randomPosition = Random.Range(0, nightPositions.Count() - 1);

        for (int i = 0; i < enemyAmount; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab, nightPositions[randomPosition].position, Quaternion.identity);
            enemy.GetComponent<Enemy>().waveEnemy = true;
        }

        yield return null;
    }
}
