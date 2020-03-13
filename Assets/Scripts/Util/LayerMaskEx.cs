using UnityEngine;

public static class LeyerMaskEx {
    public static LayerMask Without(int layer) {
        return ~(1 << layer);
    }
    public static LayerMask Only(int layer) {
        return 1 << layer;
    }
}
