import 'dart:convert';
import 'package:http/http.dart' as http;
import 'package:shared_preferences/shared_preferences.dart';
import '../models/product.dart';

class ApiService {
  // For Chrome/Web: use localhost
  // For Android emulator: use 10.0.2.2
  // For real device: use your computer's IP address
  static const String baseUrl = 'http://10.0.2.2:5000/api/mobile';
  static const String apiKey = '12341234';

  // Login and get JWT token
  Future<Map<String, dynamic>> login(String email, String password) async {
    try {
      final response = await http.post(
        Uri.parse('$baseUrl/auth/login'),
        headers: {'Content-Type': 'application/json'},
        body: jsonEncode({'usernameOrEmail': email, 'password': password}),
      );

      final data = jsonDecode(response.body);

      if (response.statusCode == 200 && data['success'] == true) {
        // Save token
        final prefs = await SharedPreferences.getInstance();
        await prefs.setString('jwt_token', data['data']['token']);
        return {'success': true};
      } else {
        return {
          'success': false,
          'message': data['errorMessage'] ?? 'Login failed',
        };
      }
    } catch (e) {
      return {'success': false, 'message': 'Connection error: $e'};
    }
  }

  // Get products list
  Future<List<Product>> getProducts({int page = 0, int pageSize = 10}) async {
    try {
      final response = await http.get(
        Uri.parse('$baseUrl/products?page=$page&pageSize=$pageSize'),
        headers: {'X-Api-Key': apiKey},
      );

      if (response.statusCode == 200) {
        final data = jsonDecode(response.body);
        if (data['success'] == true) {
          final List<dynamic> productsJson = data['data']['products'];
          return productsJson.map((json) => Product.fromJson(json)).toList();
        }
      }
      return [];
    } catch (e) {
      print('Error fetching products: $e');
      return [];
    }
  }
}
