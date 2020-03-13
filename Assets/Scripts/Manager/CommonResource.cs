using UnityEngine;

// TODO: 複数のオブジェクトから参照される可能性のあるリソース。あんまりいけてない雰囲気はある...
public class CommonResource : SingletonMonoBehaviour<CommonResource> {
    public ParticleSystem diggSnowParticle = default;
}
