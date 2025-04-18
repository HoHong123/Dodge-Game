using System.Collections.Generic;


namespace Util.Container {
    /* ===============================
     * + ��� ���� �����̳ʿ� ������ �� �ִ� ���� ��ƿ��Ƽ ��ɵ��� ���� Ŭ����
     * ===============================
     */
    public static class ContinerUtil {
        public static void Shuffle<T>(IList<T> collection) {
            System.Random rand = new System.Random();

            int n = collection.Count;
            while (n > 1) {
                n--;

                int k = rand.Next(n + 1);

                T value = collection[k];
                collection[k] = collection[n];
                collection[n] = value;
            }
        }

        public static void AddRange<TKey, TValue>(Dictionary<TKey, TValue> _existing, Dictionary<TKey, TValue> _new) {
            foreach (var kvp in _new) {
                if (!_existing.ContainsKey(kvp.Key))
                    _existing.Add(kvp.Key, kvp.Value);
            }
        }
    }
}