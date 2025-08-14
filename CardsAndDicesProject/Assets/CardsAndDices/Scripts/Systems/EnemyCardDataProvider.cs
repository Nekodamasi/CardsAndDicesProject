using System.Collections.Generic;
using VContainer;
using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// エネミーのデータ（マスターデータ、ウェーブ情報、難易度設定など）から、
    /// カード生成システムが利用するCardInitializationDataを生成する責務を負います。
    /// </summary>
    [CreateAssetMenu(fileName = "EnemyCardDataProvider", menuName = "CardsAndDices/DataProviders/EnemyCardDataProvider")]
    public class EnemyCardDataProvider : ScriptableObject, ICardDataProvider
    {
        [Inject]
        public void Initialize()
        {
        }

        // TODO: エネミーのマスターデータやウェーブ情報を参照するフィールドを追加
        // [Inject] private EnemyMasterData _enemyMaster; のような形でDIする
        // [Inject] private CombatState _combatState; のような形でDIする

        /// <summary>
        /// エネミーの現在の状態から、生成すべきカードの初期化データリストを取得します。
        /// ICardDataProviderインターフェースのGetCardDataListを実装します。
        /// </summary>
        /// <returns>エネミーのカード初期化データを含むリスト。</returns>
        public List<CardInitializationData> GetCardDataList()
        {
            // このメソッドはICardDataProviderインターフェースの要件を満たすために存在しますが、
            // 通常はGetCardDataListForWave(int waveNumber)を使用します。
            // 必要に応じて、デフォルトのウェーブ番号でGetCardDataListForWaveを呼び出すか、
            // 例外をスローすることを検討してください。
            Debug.LogWarning("EnemyCardDataProvider.GetCardDataList()が呼び出されました。通常はGetCardDataListForWave(int waveNumber)を使用してください。");
            return GetCardDataListForWave(1); // 例としてウェーブ1のデータを返す
        }

        /// <summary>
        /// 指定されたウェーブ番号に基づいて、生成すべきエネミーカードの初期化データリストを取得します。
        /// </summary>
        /// <param name="waveNumber">生成するウェーブの番号。</param>
        /// <returns>エネミーのカード初期化データを含むリスト。</returns>
        public List<CardInitializationData> GetCardDataListForWave(int waveNumber)
        {
            var list = new List<CardInitializationData>();

            // TODO: _enemyMasterから、指定されたウェーブに出現するエネミーの基本情報を取得します。
            // TODO: _combatStateから、現在の難易度設定やイベントによる補正情報を取得します。
            // TODO: 取得した情報に基づき、各エネミーカードに対応するCreatureDataとInletAbilityProfileを生成し、
            // 必要に応じて動的な変更（例: ステータス補正）を適用します。
            // TODO: 生成したCreatureDataとInletAbilityProfileを使用してCardInitializationDataのインスタンスを作成します。
            // TODO: 作成したCardInitializationDataをリストに追加し、返します。

            return list;
        }
    }
}
