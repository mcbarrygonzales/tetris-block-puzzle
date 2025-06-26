using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
   public static GameManager Instance;
   
   [FormerlySerializedAs("m_gridSystem")]
   [Header("Managers")] 
   [SerializeField] private GridManager m_gridManager;
   [SerializeField] private ShapeManager m_shapeManager;
   [SerializeField] private ScoreManager m_scoreManager;
   [SerializeField] private UIManager m_uiManager;

   private void OnEnable()
   {
      GameEvents.OnGameOver += OnGameOver;
   }

   private void OnDisable()
   {
      GameEvents.OnGameOver -= OnGameOver;
   }

   private void Awake()
   {
      if (Instance != null && Instance != this)
      {
         Destroy(gameObject);
         return;
      }
      Instance = this;
   }

   private void Start()
   {
      StartGame();
   }

   public void StartGame()
   {
      m_gridManager.InitializeGrid();
      m_scoreManager.ResetScore();
      m_uiManager.HideGameOver();
      m_shapeManager.SpawnShapes();
   }

   public void OnGameOver()
   {
      m_uiManager.ShowGameOver();
   }

   public void RestartGame()
   {
      m_gridManager.ResetGrid();
      m_scoreManager.ResetScore();
      m_uiManager.HideGameOver();
      m_shapeManager.ClearAllCurrentShapes();
      m_shapeManager.SpawnShapes();
   }
}
