using UnityEngine;
using UnityEngine.UI;
using TMPro;  // Import namespace untuk TextMeshPro
using System.Collections;

public class MejaInteraction : MonoBehaviour
{
    public Image pressEAmbil;          // UI untuk tombol ambil sampah
    public TextMeshProUGUI jumlahSampahText;  // Ganti Text menjadi TextMeshProUGUI
    public Image sampahIcon;          // Icon sampah
    public RigidbodyMovement playerMovement;  // Referensi script player

    private int jumlahSampah = 0;  // Variabel untuk jumlah sampah
    private int maxSampah = 20;  // Total sampah yang akan ditambahkan

    private void Start()
    {
        pressEAmbil.gameObject.SetActive(false);  // Tombol pressEAmbil disembunyikan
        sampahIcon.gameObject.SetActive(true);  // Menampilkan ikon sampah secara default
        jumlahSampahText.gameObject.SetActive(true); // Tampilkan jumlah sampah
        UpdateJumlahSampahText();  // Update jumlah sampah pada UI
        StartCoroutine(AddTrashRandomly());  // Mulai penambahan sampah secara random
    }

    private void Update()
    {
        // Cek apakah pressEAmbil harus tampil atau tidak
        if (!IsPlayerInCollider())
        {
            // Tampilkan jumlah sampah (ikon dan teks) tanpa pengecualian
            sampahIcon.gameObject.SetActive(true);  // Tampilkan ikon sampah
            jumlahSampahText.gameObject.SetActive(true);  // Tampilkan jumlah sampah
            pressEAmbil.gameObject.SetActive(false);  // Sembunyikan tombol pressEAmbil
        }
        else
        {
            // Jika player berada di dalam collider meja dan isHolding false
            if (!playerMovement.isHolding && jumlahSampah > 0)
            {
                pressEAmbil.gameObject.SetActive(true);  // Tombol ambil sampah aktif
                sampahIcon.gameObject.SetActive(false);  // Sembunyikan ikon sampah
                jumlahSampahText.gameObject.SetActive(false);  // Sembunyikan jumlah sampah
            }
        }

        // Cek jika tombol E ditekan dan player ada dalam collider meja
        if (Input.GetKeyDown(KeyCode.E) && pressEAmbil.gameObject.activeSelf && !playerMovement.isHolding && jumlahSampah > 0)
        {
            AmbilSampah();  // Panggil fungsi untuk mengurangi jumlah sampah
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Pastikan tombol pressEAmbil hanya tampil saat isHolding false dan ada sampah
            if (!playerMovement.isHolding && jumlahSampah > 0)
            {
                pressEAmbil.gameObject.SetActive(true);  // Tampilkan tombol pressEAmbil
                sampahIcon.gameObject.SetActive(false);  // Sembunyikan ikon jumlah sampah
                jumlahSampahText.gameObject.SetActive(false);  // Sembunyikan jumlah sampah
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Ketika player keluar dari collider, sembunyikan tombol pressEAmbil dan tampilkan UI default
            pressEAmbil.gameObject.SetActive(false);  // Sembunyikan tombol pressEAmbil
            sampahIcon.gameObject.SetActive(true);  // Tampilkan ikon sampah
            jumlahSampahText.gameObject.SetActive(true);  // Tampilkan jumlah sampah
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Update visibility saat player tetap di area dan status isHolding berubah
            if (!playerMovement.isHolding && jumlahSampah > 0)
            {
                // Pastikan hanya menampilkan pressEAmbil ketika player berada dalam collider dan tidak memegang sampah
                if (!pressEAmbil.gameObject.activeSelf)
                    pressEAmbil.gameObject.SetActive(true);
                sampahIcon.gameObject.SetActive(false);  // Sembunyikan ikon jumlah sampah
                jumlahSampahText.gameObject.SetActive(false);  // Sembunyikan jumlah sampah
            }
            else
            {
                pressEAmbil.gameObject.SetActive(false);  // Sembunyikan tombol pressEAmbil
            }
        }
    }

    // Cek apakah player berada dalam collider meja
    private bool IsPlayerInCollider()
    {
        // Return true jika player berada dalam collider meja
        return pressEAmbil.gameObject.activeSelf;
    }

    // Update jumlah sampah pada UI
    private void UpdateJumlahSampahText()
    {
        jumlahSampahText.text = jumlahSampah.ToString();  // Update jumlah sampah di TextMeshProUGUI
    }

    // Coroutine untuk menambahkan sampah secara random
    private IEnumerator AddTrashRandomly()
    {
        float elapsedTime = 0f;
        float totalTime = 120f; // 2 menit (120 detik)
        while (elapsedTime < totalTime && jumlahSampah < maxSampah)
        {
            // Tambah sampah secara random dalam waktu tertentu
            yield return new WaitForSeconds(Random.Range(3f, 6f)); // Random antara 3 detik sampai 6 detik
            if (jumlahSampah < maxSampah)
            {
                jumlahSampah++;
                UpdateJumlahSampahText();  // Update jumlah sampah di UI
            }
            elapsedTime += Random.Range(3f, 6f);  // Update waktu yang sudah berlalu
        }
    }

    // Fungsi untuk mengurangi jumlah sampah setiap kali PressEAmbil ditekan
    public void AmbilSampah()
    {
        if (jumlahSampah > 0 && !playerMovement.isHolding)
        {
            jumlahSampah--;  // Kurangi jumlah sampah
            UpdateJumlahSampahText();  // Update UI jumlah sampah
        }
    }
}