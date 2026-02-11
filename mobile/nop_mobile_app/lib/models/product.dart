class Product {
  final String name;
  final double price;
  final String priceFormatted;
  final String imageUrl;

  Product({
    required this.name,
    required this.price,
    required this.priceFormatted,
    required this.imageUrl,
  });

  factory Product.fromJson(Map<String, dynamic> json) {
    return Product(
      name: json['name'] ?? '',
      price: (json['price'] ?? 0).toDouble(),
      priceFormatted: json['priceFormatted'] ?? '',
      imageUrl: json['imageUrl'] ?? '',
    );
  }
}
